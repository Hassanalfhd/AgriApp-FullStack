export default function Footer() {
  /* 🟦 Footer */
  return (
    <footer className=" bg-gray-100 border-t mt-auto w-full">
      <div className="container mx-auto px-4 py-4 text-center text-gray-600 text-sm">
        © {new Date().getFullYear()} AgriApp. All rights reserved.
      </div>
    </footer>
  );
}
