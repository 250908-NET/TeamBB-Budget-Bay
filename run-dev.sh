#!/bin/bash

# .NET Development Startup Script
# Automates the full development workflow: restore, build, migrate, test, and run

# Configuration
SOLUTION_PATH="${PWD}"  # Parent directory containing .sln file
API_PROJECT_PATH="./BudgetBay.App/BudgetBay.Api/"  # Adjust to your API project path
TEST_PROJECT_PATH="./BudgetBay.App/BudgetBay.Test/"  # Adjust to your test project path
TEST_COVERAGE_SCRIPT="./test-report.sh"  # Path to your existing test coverage script
SWAGGER_URL="http://localhost:5202/swagger"  # Adjust port if needed
API_DIR="./BudgetBay.App/BudgetBay.Api"
CLIENT_DIR="../BudgetBay.Client"

# Vite URL
VITE_URL="http://localhost:5173"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# Function to print section headers
print_header() {
    echo ""
    echo -e "${CYAN}========================================${NC}"
    echo -e "${CYAN}$1${NC}"
    echo -e "${CYAN}========================================${NC}"
}

# Function to handle errors
handle_error() {
    echo -e "${RED}❌ Error: $1${NC}"
    echo -e "${RED}🛑 Stopping execution due to failure${NC}"
    exit 1
}

# Function to check if a command succeeded
check_success() {
    if [ $? -ne 0 ]; then
        handle_error "$1"
    fi
    echo -e "${GREEN}✅ $2${NC}"
}

# Main script starts here
echo -e "${BLUE}🚀 .NET Development Startup Script${NC}"
echo -e "${BLUE}====================================${NC}"
echo ""


# Step 1: Restore the solution
print_header "📦 Step 1: Restoring Solution"
cd "$SOLUTION_PATH" || handle_error "Solution directory not found"
dotnet restore
check_success "Failed to restore solution" "Solution restored successfully"


# Step 2: Build the application
print_header "🔨 Step 2: Building Application"
dotnet build --no-restore
check_success "Failed to build application" "Application built successfully"


# Step 3: Build the unit tests
print_header "🧪 Step 3: Building Unit Tests"
dotnet build "$TEST_PROJECT_PATH" --no-restore
check_success "Failed to build unit tests" "Unit tests built successfully"

# Step 4: Check for model changes and create migration if needed
print_header "🗄️  Step 4: Checking for Database Changes"

cd "$API_PROJECT_PATH" || handle_error "API project directory not found at: $API_PROJECT_PATH"

echo -e "${YELLOW}Checking if migration is needed...${NC}"

# Check if EF Core tools are installed
if ! command -v dotnet-ef &> /dev/null && ! dotnet ef --version &> /dev/null; then
    echo -e "${RED}⚠️  EF Core tools not found. Installing...${NC}"
    dotnet tool install --global dotnet-ef
    check_success "Failed to install EF Core tools" "EF Core tools installed"
fi

# Check EF Core version
EF_VERSION_OUTPUT=$(dotnet ef --version 2>&1)

# Extract major version only (e.g., "9" from "9.0.9")
EF_MAJOR_VERSION=$(echo "$EF_VERSION_OUTPUT" | grep -oP '\d+\.\d+\.\d+' | head -n 1 | cut -d. -f1)
echo -e "${BLUE}🔍 Debug: Detected EF Core major version: ${EF_MAJOR_VERSION:-'unknown'}${NC}"

# Check if version is 8 or higher (simple integer comparison)
if [ -n "$EF_MAJOR_VERSION" ] && [ "$EF_MAJOR_VERSION" -ge 8 ] 2>/dev/null; then
    
    # EF Core 8+ has a built-in command
    PENDING_OUTPUT=$(dotnet ef migrations has-pending-model-changes --no-build 2>&1)
    HAS_CHANGES=$?
    
    if [ $HAS_CHANGES -eq 0 ]; then
        echo -e "${YELLOW}ℹ️  No model changes detected - skipping migration${NC}"
    elif [ $HAS_CHANGES -eq 1 ]; then
        MIGRATION_NAME="AutoMigration_$(date +%Y%m%d_%H%M%S)"
        echo -e "${BLUE}🔍 Debug: Changes detected! Creating migration: $MIGRATION_NAME${NC}"
        
        MIGRATION_ADD_OUTPUT=$(dotnet ef migrations add "$MIGRATION_NAME" --no-build 2>&1)
        MIGRATION_EXIT_CODE=$?
        
        if [ $MIGRATION_EXIT_CODE -eq 0 ]; then
            echo -e "${GREEN}✅ Migration created: $MIGRATION_NAME${NC}"
        else
            handle_error "Failed to create migration. Output: $MIGRATION_ADD_OUTPUT"
        fi
    else
        handle_error "Failed to check for pending model changes. Output: $PENDING_OUTPUT"
    fi
else
    # Fallback for older EF Core versions: try to add migration
    MIGRATION_NAME="AutoMigration_$(date +%Y%m%d_%H%M%S)"
    
    # Capture output to check if migration is empty
    MIGRATION_OUTPUT=$(dotnet ef migrations add "$MIGRATION_NAME" --no-build 2>&1)
    MIGRATION_EXIT=$?
    
    if echo "$MIGRATION_OUTPUT" | grep -qi "no changes"; then
        echo -e "${YELLOW}ℹ️  No model changes detected - skipping migration${NC}"
        echo -e "${BLUE}🔍 Debug: Removing empty migration...${NC}"
        # Remove the empty migration if it was created
        dotnet ef migrations remove --force --no-build > /dev/null 2>&1
    elif [ $MIGRATION_EXIT -eq 0 ]; then
        echo -e "${GREEN}✅ New migration created: $MIGRATION_NAME${NC}"
    else
        handle_error "Failed to create migration. Output: $MIGRATION_OUTPUT"
    fi
fi

# Step 5: Update the database
print_header "📊 Step 5: Updating Database"
if [ -z "$MIGRATION_NAME" ]; then
    echo -e "${YELLOW}ℹ️  No model changes detected - skipping database update${NC}"
else
    dotnet ef database update --no-build
    check_success "Failed to update database" "Database updated successfully"
fi


# Step 6: Run unit tests with coverage
print_header "🧪 Step 6: Running Unit Tests with Coverage"
cd "$SOLUTION_PATH" || handle_error "Cannot navigate back to solution directory"

if [ ! -f "$TEST_COVERAGE_SCRIPT" ]; then
    handle_error "Test coverage script not found at: $TEST_COVERAGE_SCRIPT"
fi

chmod +x "$TEST_COVERAGE_SCRIPT"
bash "$TEST_COVERAGE_SCRIPT" "$SOLUTION_PATH"
check_success "Unit tests failed" "Unit tests passed with coverage report generated"

# -----------------------------
# Start API in background
# -----------------------------
print_header "🌐 Starting .NET API in background"
cd "$API_DIR" || { echo "API directory not found"; exit 1; }
dotnet run --project ./BudgetBay.Api.csproj &
API_PID=$!
echo -e "${GREEN}✅ API started in background with PID $API_PID${NC}"

# -----------------------------
# Start Vite in foreground
# -----------------------------
print_header "⚡ Starting Vite React Client"
pwd
cd "$CLIENT_DIR" || { echo "Client directory not found"; exit 1; }

# Install dependencies silently
npm install --silent

# Open browser after a short delay
(
  sleep 5
  echo -e "${BLUE}🌐 Opening Vite client in browser at $VITE_URL...${NC}"
  if command -v xdg-open > /dev/null; then
      xdg-open "$VITE_URL" &
  elif command -v open > /dev/null; then
      open "$VITE_URL" &
  elif command -v start > /dev/null; then
      start "$VITE_URL" &
  else
      echo -e "${YELLOW}⚠️ Could not automatically open browser. Navigate manually to: $VITE_URL${NC}"
  fi
) &

# Run Vite in foreground
npm run dev -- --host

# -----------------------------
# Cleanup API after Vite exits
# -----------------------------
print_header "🛑 Stopping API"
kill $API_PID 2>/dev/null
echo -e "${GREEN}✅ API stopped. Development servers terminated.${NC}"