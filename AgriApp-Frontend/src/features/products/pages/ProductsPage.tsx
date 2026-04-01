import { useCallback, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import type { TProducts } from "../types/IProductsResponse";
import { Fab } from "@/shared/components/ui/Fab";
import { ROUTES } from "@/shared/constants/ROUTESLinks";
import GenericGridList from "@/shared/components/generic/ListComponets/GenericGridList";
import SkeletonGrid from "@/shared/components/ui/SkeletonGrid";
import { getProductsThunk } from "../actThunks/productsThunks";
import { DEFAULT_PAGE_SIZE } from "@/shared/constants/Config";
import ProductCard from "../components/ProductCard";

function ProductsPage() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const { loading, error, lastFetched, hasMore } = useAppSelector(
    (state) => state.products,
  );
  const { items, pageSize, page } = useAppSelector((s) => s.products.Data);

  const loadProducts = useCallback(() => {
    if (loading === "pending" || !hasMore) return;
    dispatch(getProductsThunk({ page, pageSize }));
  }, [dispatch, page, hasMore, loading, pageSize]);

  // Initial Fetch logic
  useEffect(() => {
    const shouldFetch =
      items.length === 0 ||
      !lastFetched ||
      Date.now() - lastFetched > 60 * 1000;

    if (shouldFetch) loadProducts();
  }, [items.length, lastFetched, loadProducts]);

  // Infinite Scroll Listener
  useEffect(() => {
    const handleScroll = () => {
      if (
        window.innerHeight + window.scrollY >=
          document.body.offsetHeight - 500 &&
        loading !== "pending" &&
        hasMore
      ) {
        loadProducts();
      }
    };

    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, [loadProducts, loading, hasMore]);

  const renderItem = useCallback(
    (p: TProducts) => (
      <ProductCard
        key={p.id}
        {...p} // Spread props for cleaner code if keys match
      />
    ),
    [],
  );

  const handleFabBtn = () => navigate(`${ROUTES.ADMIN.PRODUCTS.ADD}`);

  return (
    <main className="container mx-auto px-4 py-8">
      {/* Header Section */}
      <header className="flex justify-between items-end mb-8">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Our Products</h1>
          <p className="text-gray-500 mt-1">Discover our latest collection</p>
        </div>
      </header>

      {/* Grid Content */}
      <section aria-label="Products List">
        <GenericGridList
          items={items}
          renderItem={renderItem}
          gap="8"
          cols={{ base: 1, sm: 2, md: 3, lg: 4, xl: 4 }}
        />
      </section>

      {/* Loading States & Skeletons */}
      <section className="mt-10" aria-live="polite">
        {loading === "pending" && (
          <>
            <p className="sr-only">Loading more products...</p>
            <SkeletonGrid count={DEFAULT_PAGE_SIZE} />
          </>
        )}

        {/* Status Messages */}
        {!hasMore && items.length > 0 && (
          <div className="text-center py-12 border-t border-gray-100 mt-8">
            <p className="text-gray-400 font-medium">
              ✨ You've reached the end! No more products to show.
            </p>
          </div>
        )}

        {loading === "failed" && (
          <div
            role="alert"
            className="text-center p-6 bg-red-50 rounded-xl border border-red-100 mt-4"
          >
            <p className="text-red-600 font-semibold">Error: {error}</p>
            <button
              onClick={loadProducts}
              className="mt-2 text-sm text-red-700 underline hover:no-underline"
            >
              Try again
            </button>
          </div>
        )}
      </section>

      {/* Float Action Button */}
      <Fab icon="+" onClick={handleFabBtn} aria-label="Add new product" />
    </main>
  );
}

export default ProductsPage;
