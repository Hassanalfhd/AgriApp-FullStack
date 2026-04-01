import { useEffect, useState, type ReactNode } from "react";
import { ChevronLeft, ChevronRight } from "lucide-react";

interface DetailsPageProps {
  title: string;
  images?: string[];
  description?: string;
  extraInfo?: ReactNode;
}

export default function GenericDetailsPage({
  title,
  images = [],
  description,
  extraInfo,
}: DetailsPageProps) {
  const [currentIndex, setCurrentIndex] = useState(0);
  const [isPaused, setIsPaused] = useState(false);

  useEffect(() => {
    if (images.length <= 1 || isPaused) return;

    const interval = setInterval(() => {
      setCurrentIndex((prev) => (prev + 1) % images.length);
    }, 4000); // Slightly slower for better readability

    return () => clearInterval(interval);
  }, [images, isPaused]);

  const handleNext = () =>
    setCurrentIndex((prev) => (prev + 1) % images.length);
  const handlePrev = () =>
    setCurrentIndex((prev) => (prev === 0 ? images.length - 1 : prev - 1));

  return (
    <main className="min-h-screen bg-gray-50 py-8 px-4 sm:px-6">
      <article className="max-w-4xl mx-auto bg-white rounded-3xl shadow-xl border border-gray-100 overflow-hidden">
        {/* ✅ Accessible Image Carousel */}
        {images.length > 0 && (
          <section
            className="relative group h-72 sm:h-[400px] bg-gray-200"
            aria-roledescription="carousel"
            aria-label="Product Images"
            onMouseEnter={() => setIsPaused(true)}
            onMouseLeave={() => setIsPaused(false)}
          >
            {images.map((img, i) => (
              <div
                key={i}
                role="group"
                aria-roledescription="slide"
                aria-label={`${i + 1} of ${images.length}`}
                aria-hidden={i !== currentIndex}
                className={`absolute inset-0 transition-opacity duration-1000 ease-in-out ${
                  i === currentIndex ? "opacity-100 z-10" : "opacity-0 z-0"
                }`}
              >
                <img
                  src={img}
                  alt={`${title} view ${i + 1}`}
                  className="w-full h-full object-cover"
                />
              </div>
            ))}

            {/* Carousel Controls - Only show if multiple images */}
            {images.length > 1 && (
              <>
                <div className="absolute inset-0 flex items-center justify-between p-4 z-20">
                  <button
                    onClick={handlePrev}
                    aria-label="Previous image"
                    className="p-2 rounded-full bg-white/80 hover:bg-white text-gray-800 shadow-lg backdrop-blur-sm transition-all focus:ring-2 focus:ring-blue-500 outline-none"
                  >
                    <ChevronLeft size={24} />
                  </button>
                  <button
                    onClick={handleNext}
                    aria-label="Next image"
                    className="p-2 rounded-full bg-white/80 hover:bg-white text-gray-800 shadow-lg backdrop-blur-sm transition-all focus:ring-2 focus:ring-blue-500 outline-none"
                  >
                    <ChevronRight size={24} />
                  </button>
                </div>

                {/* Indicators */}
                <div
                  className="absolute bottom-4 left-1/2 -translate-x-1/2 flex gap-2 z-20"
                  aria-label="Image selection"
                >
                  {images.map((_, i) => (
                    <button
                      key={i}
                      onClick={() => setCurrentIndex(i)}
                      aria-label={`Go to slide ${i + 1}`}
                      aria-current={i === currentIndex ? "true" : "false"}
                      className={`h-2 rounded-full transition-all ${
                        i === currentIndex ? "w-6 bg-white" : "w-2 bg-white/50"
                      }`}
                    />
                  ))}
                </div>
              </>
            )}
          </section>
        )}

        {/* ✅ Content Section */}
        <div className="p-8 sm:p-12">
          <header className="mb-6">
            <h1 className="text-3xl sm:text-4xl font-extrabold text-gray-900 leading-tight">
              {title}
            </h1>
            <div
              className="h-1.5 w-20 bg-blue-600 mt-4 rounded-full"
              aria-hidden="true"
            />
          </header>

          {description && (
            <section aria-labelledby="desc-title">
              <h2 id="desc-title" className="sr-only">
                Description
              </h2>
              <p className="text-gray-600 text-lg leading-relaxed mb-8">
                {description}
              </p>
            </section>
          )}

          {/* ✅ Additional Information Section */}
          {extraInfo && (
            <section className="mt-8 pt-8 border-t border-gray-100">
              <h2 className="text-xl font-bold text-gray-800 mb-6">
                Details & Specifications
              </h2>
              <div className="prose prose-blue max-w-none text-gray-700">
                {extraInfo}
              </div>
            </section>
          )}
        </div>
      </article>

      {/* Screen Reader Live Region: Announces slide changes */}
      <div className="sr-only" aria-live="polite" aria-atomic="true">
        Showing image {currentIndex + 1} of {images.length}
      </div>
    </main>
  );
}
