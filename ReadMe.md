# 🌾 AgriApp: End-to-End Agricultural Ecosystem

[![Full Stack](https://img.shields.io/badge/Full%20Stack-Dotnet%208%20%2B%20React-blue?style=for-the-badge)](https://github.com/hasan-alfahd)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20%26%20Feature--Based-green?style=for-the-badge)](#-system-architecture)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](LICENSE)

**AgriApp** is a robust, enterprise-grade platform designed to bridge the gap between farmers and consumers. It streamlines agricultural commerce through a unified system featuring role-based dashboards, real-time order tracking, and advanced administrative management.

---

## 🏗️ System Architecture

The ecosystem is built on a **Decoupled Architecture**, ensuring high performance, maintainability, and independent scalability for both the client and the server.

1.  **Frontend (AgriApp-Client):** A modern SPA built with **React 18**, utilizing a **Feature-Based Architecture** to organize domain-specific logic and improve scalability.
2.  **Backend (AgriApp-API):** A high-performance server built with **ASP.NET Core 8** following **Clean Architecture** principles to ensure strict separation of concerns.

---

## 📂 Project Structure

This repository contains two primary modules:

| Directory                                       | Component          | Primary Tech Stack                        |
| :---------------------------------------------- | :----------------- | :---------------------------------------- |
| [**`📂 AgriApp-Frontend`**](./AgriApp-Frontend) | UI/UX & Dashboards | React, Redux Toolkit, Tailwind CSS, Axios |
| [**`📂 AgriApp-Backend`**](./AgriApp-Backend)   | Core API & Logic   | .NET 8, SQL Server, Dapper, EF Core 9     |

---

## 🌟 Key Functional Highlights

- **Multi-Role Ecosystem:** Specialized experiences for **Customers** (Shopping), **Farmers** (Inventory & Sales), and **Admins** (System Governance).
- **Secure Transactions:** Role-based access control (RBAC) powered by optimized **JWT Authentication**.
- **Hybrid Data Access:** Leveraging **EF Core** for complex relationships and **Dapper** for lightning-fast analytical reporting.
- **Operational Integrity:** Integrated **Audit Logging** system and automated background services for resource cleanup.

---

## 🛠️ Technical Competencies

### **Backend Engineering**

- **Clean Architecture:** Implementation of API, Business Logic (BLL), and Data Access (DAL) layers.
- **Patterns:** Result Pattern, Repository Pattern, and Unit of Work.
- **Performance:** SQL Server optimization, Rate Limiting, and Global Error Handling.
- **Monitoring:** Custom Middleware for profiling and request tracking.

### **Frontend Excellence**

- **State Management:** Centralized and persistent state using **Redux Toolkit**.
- **Routing:** Advanced route guards (`ProtectedRoute`, `RoleGuard`) for secure navigation.
- **Optimization:** Code splitting (Lazy Loading) and memoization to reduce initial load times.

---

## 🚀 Installation & Setup

To get the full ecosystem running locally, follow these steps:

### 1️⃣ Clone the Repository

```bash
git clone <your-repo-url>

2️⃣ Backend Setup
cd AgriApp-Backend
# Update ConnectionStrings in appsettings.json
dotnet ef database update --project Agricultural_For_CV_DAL --startup-project Agricultural_For_CV
dotnet run --project Agricultural_For_CV

3️⃣ Frontend Setup
cd AgriApp-Frontend
npm install
npm run dev

👨‍💻 Author
Hasan Ameen Alfahd
Full-Stack Web Developer & IT Specialist

A passionate developer dedicated to building scalable, user-centric web applications. This project serves as a comprehensive demonstration of my skills in modern web architecture and full-stack integration.

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://linkedin.com/in/Hassanalfhd)
[![GitHub](https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white)](https://github.com/Hassanalfhd)
```
