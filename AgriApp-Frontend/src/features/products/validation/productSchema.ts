import * as Yup from "yup";
export const productValidationSchema = Yup.object().shape({
  name: Yup.string().required("Product name is required."),
  description: Yup.string().optional(),
  quantityInStock: Yup.number().min(0).required("Type is required. "),
  price: Yup.number().min(0).required("price is required."),
  cropTypeId: Yup.number().moreThan(0, "select product type.").required(),
  quantityTypeId: Yup.number().moreThan(0, "Select Unit Type.").required(),
});
