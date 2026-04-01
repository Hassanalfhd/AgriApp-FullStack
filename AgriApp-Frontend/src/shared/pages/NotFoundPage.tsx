import { Link } from "react-router-dom";

export default function NotFoundPage() {
  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-50 text-gray-700">
      <h1 className="text-8xl font-extrabold text-green-600">404</h1>
      <h2 className="text-2xl font-semibold mt-4">Page Not Found.</h2>
      <p className="text-gray-500 mt-2">
        We couldn't find the page you were looking for.
      </p>
      <Link
        to="/"
        className="mt-6 inline-block px-6 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition"
      >
        Back to Home
      </Link>
    </div>
  );
}
