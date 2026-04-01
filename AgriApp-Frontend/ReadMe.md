# 🌱 AgriApp Frontend

A modern, scalable frontend application for an agricultural platform that connects **customers, farmers, and administrators** through a unified system with role-based dashboards and real-time interactions.

---

## 📌 Overview

AgriApp is a multi-role platform designed to simplify agricultural commerce and management.

- **Customers** can browse products, explore crops, and place orders
- **Farmers** can manage products and track incoming orders
- **Admins** have full control over the system, including user management and reporting

The application focuses on delivering a **clean user experience**, **scalable architecture**, and **high performance**.

---

## 🚀 Key Features

### 👥 Multi-Role System

- Role-based access control (Customer / Farmer / Admin)
- Dynamic dashboards based on user role

### 🛒 Customer Experience

- Browse products, categories, and crops
- Add items to cart and place orders
- View order history

### 🌾 Farmer Dashboard

- Manage products (create, update)
- Track and manage orders
- View reports (e.g., monthly sales)

### 🛠️ Admin Panel

- Manage users (activate / deactivate)
- Control system data (categories, crops)
- Access system-wide reports

---

## 🧭 Application Flow

### Customer Flow

- Browse → View Product → Add to Cart → Place Order

### Farmer Flow

- Dashboard → Manage Products → Handle Orders → View Reports

### Admin Flow

- Dashboard → Manage Users → Monitor System

---

## 🏗️ Frontend Architecture

The application follows a **feature-based architecture** combined with reusable component design:

- **Feature Components** → Domain-specific logic (products, orders, users)
- **Shared Components** → Reusable UI across the app
- **UI Components** → Consistent design system
- **Layouts** → Structured page layouts (dashboard, public pages)

This structure improves **scalability**, **maintainability**, and **code organization**.

---

## 🧠 State Management

- Centralized state management using **Redux Toolkit**
- Feature-based slices:
  - Auth
  - Products
  - Categories
  - Crops
  - Orders (Customer & Farmer)
  - Reports

- Persistent authentication using **redux-persist**

This ensures predictable state handling and better separation of concerns.

---

## 🔗 API Integration

- API communication handled using **Axios**
- Centralized API client for consistency
- Abstracted service layer for reusable API calls
- Generic CRUD service for common operations

---

## 🛡️ Routing & Access Control

- Role-based routing system
- Protected routes for authenticated users
- Route separation:
  - Public routes
  - Client routes
  - Dashboard routes

Custom guards:

- `ProtectedRoute`
- `RoleGuard`
- `AuthRedirect`

---

## 🎨 Styling & UI System

- Built with **Tailwind CSS** (utility-first approach)
- Centralized layout configuration
- Responsive sidebar with dynamic states:
  - Collapsed / Expanded

- Consistent UI design across all pages

---

## ⚡ Performance Optimization

- Lazy loading for pages (code splitting)
- Optimized rendering using memoization
- Reduced initial load time

---

## 💡 User Experience (UX)

- Global notification system using toast messages
- Loading states for async operations
- Error handling for API requests
- Smooth and responsive UI interactions

---

## 📂 Project Structure

![Featuer-Based](./screenshots/Featuer-based.gif)

---

## 📸 Screenshots

## 🛠️ Admin Panel

### Dashboard Overview

![Admin Dashboard](./screenshots/AdminDashboard.gif)
![Admin Dashboard](./screenshots/AdminDashboardForMobile.gif)

### User Management

![User Management](./screenshots/UserManagmentByAdmin.png)

### Products Management

![Products Management](./screenshots/ProdcutPageInDashboardByAdmin.png)

### 🌾 Farmer Dashboard

- Manage orders and track their status (Preparing, On the way, Cancelled)
- Monitor order inventory and details

## 🌾 Farmer Dashboard

- View and manage orders with clear status indicators
- Display quantity, price, user, and date information
  ![Farmer Orders](./screenshots/FarmerDashboard.gif)

### Orders Management

![Farmer Orders](./screenshots/FarmerOrdersDashboard.png)

### Accepted Orders

![Accepted Orders](./screenshots/FarmerOrdersAcceptedDashboard.png)

### Add Product

![Add Product](./screenshots/AddNewProduct.png)

### Delete Product

![Delete Product](./screenshots/DeleteProduct.png)

## 🛒 Customer Experience

### Account Creation

![Create Account](./screenshots/CreateAccountOfCustomer.gif)

### Browse Products and Placed Order

![Products](./screenshots/CustomerPlacedOrder.gif)

### Customer Orders

![Pagination](./screenshots/CustomerOrders.gif)

## 🎨 UI & Layout

### Sidebar (Expanded)

![Sidebar Expanded](./screenshots/SideBarOfDashboard_1.png)

### Sidebar (Collapsed)

![Sidebar Collapsed](./screenshots/SideBarOfDashboard_2.png)

### User Profile

![User Profile](./screenshots/UserProfile.png)

## ▶️ Getting Started

```bash
git clone https://github.com/Hassanalfhd/AgriApp-FullStack.git
cd agriapp-frontend
npm install
npm run dev
```

---

## 📌 Future Improvements

- 🌍 Multi-language support
- 📱 Mobile optimization
- 📊 Advanced analytics dashboards
- 🔔 Real-time notifications (WebSockets)

---

## 👨‍💻 Author

**Hasan Ameen Alfahd** _IT Specialist & Full-Stack Developer_

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://linkedin.com/in/Hassanalfhd)
[![GitHub](https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white)](https://github.com/Hassanalfhd)
