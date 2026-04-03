# 📊 Database Requirement for Reports

## ⚠️ Important Notice

This project uses **SQL Server Stored Procedures** for generating advanced reports such as:

- Top Farmers Report
- Sales Growth Analysis
- Low Stock Alerts
- Farmer Monthly Sales Report
- Category Sales Analysis

---

## 🧱 Database Setup Required

To ensure the application works correctly, you **must restore the database** before running the project.

### 📥 Steps:

1. Download the database backup file (`.bak`) from the repository (or provided source).
2. Open **SQL Server Management Studio (SSMS)**.
3. Restore the database using:
   - Right-click on `Databases`
   - Choose `Restore Database`
   - Select the `.bak` file

4. Make sure the database name is:

```
AgriculturalDB_For_CV
```

```bash

restore database AgriculturalDB_For_CV
from disk = <Your Path>;
```

---

## ⚙️ Stored Procedures Dependency

The backend depends on several stored procedures that are already included in the database:

- `sp_GetTopFarmersReport`
- `sp_GetSalesGrowthReport`
- `sp_GetLowStockAlerts`
- `sp_GetFarmerMonthlySalesReport`
- `sp_GetCategorySalesAnalysis`

> 🚫 Without restoring the database, these procedures will not exist and the application will fail.

---

## 🔌 Connection String

After restoring the database, update your `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=AgriculturalDB_For_CV;Trusted_Connection=True;"
}
```

---

## ✅ Final Note

This project relies heavily on **database-level logic (Stored Procedures)** for performance and reporting.

Make sure the database is properly restored before running the application.

---

## 🚀 Optional

If you prefer, you can manually create the stored procedures using the SQL scripts provided in the project.

---
