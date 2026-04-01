import { useEffect } from "react";
import {
  useAppDispatch,
  useAppSelector,
} from "../../../shared/hooks/StoreHook";
import { getAllCropsThunk } from "../actThunks/getAllCropsThunk";
import Loader from "@/shared/components/ui/Loader";
import CropCard from "../components/CropCard";
import GenericGridList from "@/shared/components/generic/ListComponets/GenericGridList";

function CropsPage() {
  const dispatch = useAppDispatch();

  const { items, loading, error } = useAppSelector((state) => state.crops);

  useEffect(() => {
    if (items.length == 0) dispatch(getAllCropsThunk());
  }, [dispatch, items.length]);

  if (loading == "pending") return <Loader />;

  if (loading == "failed") return <div>{error}</div>;

  return (
    <>
      <GenericGridList
        items={items}
        renderItem={(c) => (
          <CropCard
            id={c.id}
            name={c.name}
            categoryName={c.categoryName}
            imagePath={c.imagePath}
            categoryId={c.categoryId}
            createdAt={c.createdAt}
            ownerId={c.ownerId}
            username={c.username}
          />
        )}
      />
    </>
  );
}

export default CropsPage;
