export const layoutClasses = {
  sidebar: {
    base: "fixed inset-y-0 left-0 z-40 bg-green-700 text-white transform transition-transform duration-300 flex flex-col ",

    // يعمل فقط في الجوال
    open: "translate-x-0 lg:translate-x-0",
    closed: "-translate-x-full lg:translate-x-0",

    collapsed: "lg:w-25 w-35  ",
    expanded: "lg:w-60 xl:w-64 ",

    header: "flex items-center justify-between p-3 border-b border-green-600",
    link: "flex items-center gap-2 py-2 px-4 rounded hover:bg-green-600/80 transition-colors text-sm",
    logout:
      "flex items-center gap-2 text-sm hover:text-gray-200 transition w-full mt-auto px-3 py-4 border-t border-green-600",
  },

  main: {
    default: " flex-1 h-full transition-all duration-200 p-4 sm:p-6 md:p-8",
    collapsed: "lg:ml-15",
    expanded: "lg:ml-23",
  },

  header:
    "flex items-center justify-between bg-white border-b px-4 py-3 shadow-sm relative",
  button: {
    base: "p-1 rounded hover:bg-green-600/80",
  },
};
