import SkeletonLoading from "./SkeletonLoading"; // استدعاء الـ Skeleton العادي

interface SkeletonGridProps {
  count?: number; // عدد الـ Skeletons اللي يظهروا
}

export default function SkeletonGrid({ count = 6 }: SkeletonGridProps) {
  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
      {Array.from({ length: count }).map((_, index) => (
        <SkeletonLoading key={index} />
      ))}
    </div>
  );
}
