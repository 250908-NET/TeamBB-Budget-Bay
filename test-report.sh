#!/bin/bash

# .NET Test Coverage Report Generator
# Usage: ./test-coverage.sh [test-project-path]

# Configuration
TEST_PROJECT_PATH=${1:-"./tests"}  # Default to ./tests if no path provided
COVERAGE_OUTPUT_DIR="./coverage"
COVERAGE_FILE="$COVERAGE_OUTPUT_DIR/coverage.cobertura.xml"
HTML_REPORT_DIR="$COVERAGE_OUTPUT_DIR/html"
HTML_REPORT_FILE="$HTML_REPORT_DIR/index.html"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}🧪 .NET Test Coverage Report Generator${NC}"
echo "=========================================="

# Check if test project directory exists
if [ ! -d "$TEST_PROJECT_PATH" ]; then
    echo -e "${RED}❌ Error: Test project directory '$TEST_PROJECT_PATH' not found${NC}"
    echo "Usage: $0 [test-project-path]"
    exit 1
fi

echo -e "${YELLOW}📁 Test project path: $TEST_PROJECT_PATH${NC}"

# Navigate to test project directory
cd "$TEST_PROJECT_PATH" || exit 1

# Create coverage output directory
mkdir -p "$COVERAGE_OUTPUT_DIR"

echo -e "${BLUE}🔧 Installing/updating coverage tools...${NC}"

# Install coverlet as a global tool if not already installed
dotnet tool install --global coverlet.console 2>/dev/null || dotnet tool update --global coverlet.console

# Install ReportGenerator as a global tool if not already installed
dotnet tool install --global dotnet-reportgenerator-globaltool 2>/dev/null || dotnet tool update --global dotnet-reportgenerator-globaltool

echo -e "${BLUE}🏃 Running tests with coverage collection...${NC}"

# Run tests with coverage collection
dotnet test \
    --collect:"XPlat Code Coverage" \
    --results-directory:"$COVERAGE_OUTPUT_DIR" \
    --logger:"console;verbosity=detailed" \
    -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura \
    &> /dev/null

# Check if tests ran successfully
if [ $? -ne 0 ]; then
    echo -e "${RED}❌ Tests failed!${NC}"
    exit 1
fi

echo -e "${GREEN}✅ Tests completed successfully!${NC}"

# Find the coverage file (it gets created in a subdirectory with a GUID)
COVERAGE_FILE=$(find "$COVERAGE_OUTPUT_DIR" -name "*.cobertura.xml" | head -1)

if [ -z "$COVERAGE_FILE" ]; then
    echo -e "${RED}❌ Error: Coverage file not found${NC}"
    exit 1
fi

echo -e "${BLUE}📊 Generating HTML coverage report...${NC}"

# Generate HTML report using ReportGenerator
reportgenerator \
    -reports:"$COVERAGE_FILE" \
    -targetdir:"$HTML_REPORT_DIR" \
    -reporttypes:Html \
    -title:"$TEST_PROJECT_PATH" \
    -tag:"$(date +%Y-%m-%d_%H-%M-%S)" \
    -classfilters:"-*Migrations*;-*Migration;-*.Migrations.*;-*DbContext*" \
    &> /dev/null

# Check if report generation was successful
if [ $? -ne 0 ]; then
    echo -e "${RED}❌ Error: Failed to generate HTML report${NC}"
    exit 1
fi

echo -e "${GREEN}✅ HTML coverage report generated successfully!${NC}"
echo -e "${YELLOW}📍 Report location: $HTML_REPORT_DIR${NC}"

# Open the report in the default browser
echo -e "${BLUE}🌐 Opening coverage report in browser...${NC}"

# Cross-platform browser opening
if command -v xdg-open > /dev/null; then
    # Linux
    xdg-open "$HTML_REPORT_FILE"
elif command -v open > /dev/null; then
    # macOS
    open "$HTML_REPORT_FILE"
elif command -v start > /dev/null; then
    # Windows (Git Bash, WSL)
    start "$HTML_REPORT_FILE"
else
    echo -e "${YELLOW}⚠️  Could not automatically open browser. Please open: $HTML_REPORT_FILE${NC}"
fi

echo -e "${GREEN}🎉 Coverage report ready! Check your browser.${NC}"