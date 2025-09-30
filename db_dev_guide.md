
# Project Development Guide

This guide covers the full workflow for local database setup, Git branching, and creating Entity Framework migrations. Following these steps will help prevent conflicts and maintain a clean project history.

---

## 1. Initial Local Database Setup

First-time setup for your local database.

### How to add Connection String:

Run this command in the project's root directory. Replace `<password>` with your actual SQL Server 'sa' password.

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=127.0.0.1,1433;Database=budgetbay_local;User Id=sa;Password=<password>;TrustServerCertificate=True;"
```

### Apply All Existing Migrations:

Create the database and apply all existing migrations.

```bash
dotnet ef database update
```

-----

## 2\. Syncing Your Branch and Creating a Migration

Follow this process **before you create a new migration** to ensure your branch is up-to-date with the `main` branch. This is the standard workflow to prevent conflicts.

1.  **Commit Your Current Work:** Make sure any changes on your feature branch are committed.

    ```bash
    git add .
    git commit -m "Your work-in-progress commit message"
    ```

2.  **Sync with the `main` Branch:**

      * Switch to `main` and pull the latest changes.
        ```bash
        git checkout main
        git pull origin main
        ```
      * Switch back to your feature branch and rebase it on top of `main`.
        ```bash
        git checkout your-feature-branch
        git rebase main
        ```

3.  **Apply Your Team's Migrations:** After rebasing, your project might now include new migrations from your teammates. Apply them to your local database.

    ```bash
    dotnet ef database update
    ```

4.  **Create Your New Migration:** Now that your local database is synced with the latest schema, you can safely create your own migration.

    ```bash
    dotnet ef migrations add <DescriptiveMigrationName>
    ```

5.  **Commit the New Migration:** The previous command created new files. You must add and commit them.

    ```bash
    git add .
    git commit -m "feat: Add migration for <DescriptiveMigrationName>"
    ```

6.  **Apply Your Own Migration:** (Optional but recommended) Test your new migration by applying it to your local database.

    ```bash
    dotnet ef database update
    ```

7.  **Push Your Changes:** Push your rebased branch, including the new migration commit, to the remote repository.

    ```bash
    git push
    ```