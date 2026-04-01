export default function SkeletonLoading() {
  return (
    <div className="bg-white rounded-2xl shadow-md p-4 flex flex-col h-full animate-pulse">
      {/* info */}
      <div className="flex items-center justify-between mb-2">
        <div className="w-16 h-3 bg-gray-200 rounded"></div>
        <div className="w-10 h-10 bg-gray-200 rounded-full"></div>
      </div>
      {/* الصورة */}
      <div className="w-full h-48 bg-gray-200 mb-3 rounded-lg"></div>

      {/* اسم المنتج */}
      <div className="w-3/4 h-5 bg-gray-200 rounded mb-1"></div>

      {/* وصف المنتج */}
      <div className="w-full h-3 bg-gray-200 rounded mb-1"></div>
      <div className="w-5/6 h-3 bg-gray-200 rounded mb-3"></div>

      {/* السعر والزر */}
      <div className="flex items-center justify-between mt-auto">
        <div className="w-20 h-5 bg-gray-200 rounded"></div>
        <div className="w-24 h-8 bg-gray-200 rounded"></div>
      </div>
    </div>
  );
}
