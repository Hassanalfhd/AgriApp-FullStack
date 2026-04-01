import * as yup from "yup";

export const registerSchema = yup.object({
  fullName: yup
    .string()
    .min(3, "Full name must be at least 3 characters")
    .required("Full name is required"),

  username: yup
    .string()
    .min(3, "Username must be at least 3 characters")
    .required("Username is required"),

  phone: yup
    .string()
    .min(12, "Phone must be at least 12 characters like(+967xxxxxxxxx)")
    .max(14, "Phone must be  less than  13 characters like(+967xxxxxxxxx)")
    .required("Phone is required"),

  email: yup
    .string()
    .email("Invalid email address")
    .required("Email is required"),

  password: yup
    .string()
    .min(8, "Password must be at least 8 characters")
    .matches(/[A-Z]/, "Password must contain an uppercase letter")
    .matches(/[a-z]/, "Password must contain a lowercase letter")
    .matches(/[0-9]/, "Password must contain a number")
    .required("Password is required"),

  userType: yup.number().required(),
});
