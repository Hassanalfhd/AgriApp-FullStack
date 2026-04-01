import { Link } from "react-router-dom";

export default function ForbiddenPage() {
  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-50 text-gray-700">
      <h1 className="text-8xl font-extrabold text-red-600">403</h1>
      <h2 className="text-2xl font-semibold mt-4">غير مسموح بالوصول</h2>
      <p className="text-gray-500 mt-2">
        ليس لديك صلاحية للوصول إلى هذه الصفحة.
      </p>
      <Link
        to="/"
        className="mt-6 inline-block px-6 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition"
      >
        العودة إلى الصفحة الرئيسية
      </Link>
    </div>
  );
}
