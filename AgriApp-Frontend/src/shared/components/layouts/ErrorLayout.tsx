import {
  Link,
  Outlet,
  useRouteError,
  isRouteErrorResponse,
} from "react-router-dom";
import { useEffect, useMemo } from "react";

export default function ErrorLayout() {
  const error = useRouteError();

  // تنظيف تلقائي عند unmount (اختياري)
  useEffect(() => {
    return () => {
      // يمكن وضع أي تنظيف هنا إذا كان هناك بيانات مؤقتة
      // React garbage collection سيتعامل مع DOM و state تلقائياً
    };
  }, []);

  // تحديد خصائص الخطأ بشكل آمن مع TypeScript
  const { status, title, message, bgColor, textColor } = useMemo(() => {
    let statusCode: number | string = "❌";
    let errorTitle = "";
    let errorMessage = "";
    let background = "bg-gray-100";
    let color = "text-gray-700";

    if (isRouteErrorResponse(error)) {
      statusCode = error.status;
      errorTitle = error.status.toString();
      errorMessage = error.statusText || "An unexpected error occurred.";
    } else if (error instanceof Error) {
      statusCode = "❌";
      errorTitle = "Error";
      errorMessage = error.message;
    }

    switch (statusCode) {
      case 403:
        errorTitle = "403";
        errorMessage = "Forbidden.";
        background = "bg-red-50";
        color = "text-red-600";
        break;
      case 404:
        errorTitle = "404";
        errorMessage = "Page Does Not Found.";
        background = "bg-yellow-50";
        color = "text-yellow-600";
        break;
      case 500:
        errorTitle = "500";
        errorMessage =
          "An error occurred on the server. Please try again later.";
        background = "bg-red-50";
        color = "text-red-600";
        break;
      default:
        break;
    }

    return {
      status: statusCode,
      title: errorTitle,
      message: errorMessage,
      bgColor: background,
      textColor: color,
    };
  }, [error]);

  return (
    <div
      className={`flex flex-col items-center justify-center min-h-screen ${bgColor} p-4`}
    >
      <h1 className={`text-8xl font-extrabold mb-4 ${textColor}`}>{title}</h1>
      <h2 className="text-2xl font-semibold mb-2">{message}</h2>
      {status && <p className="text-gray-500 mb-4">Error Code: {status}</p>}
      <h2>dsflajsdlkfksakd </h2>
      <Link
        to="/"
        className="mt-6 inline-block px-6 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition"
      >
        Back to Home
      </Link>

      {/* Outlet سيزال من الذاكرة عند unmount */}
      <Outlet />
    </div>
  );
}
