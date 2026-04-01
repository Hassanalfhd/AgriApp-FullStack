import { useEffect } from "react";
import Loader from "../../../shared/components/ui/Loader";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import { getAllCategoriesThunk } from "../actThunks/getAllCategoriesThunk";
import CategoryCard from "../components/CagegoryCard";
import GenericGridList from "@/shared/components/generic/ListComponets/GenericGridList";

function CategoriesPage() {
  const dispatch = useAppDispatch();

  const { items, loading, error } = useAppSelector((state) => state.categories);

  useEffect(() => {
    if (items.length == 0) dispatch(getAllCategoriesThunk());
  }, [dispatch, items.length]);

  if (loading == "pending") return <Loader />;

  if (loading == "failed") return <div>{error}</div>;

  return (
    <>
      <GenericGridList
        items={items}
        renderItem={(c) => (
          <CategoryCard id={c.id} name={c.name} imageFile={c.imageFile} />
        )}
      />
    </>
  );
}

export default CategoriesPage;
