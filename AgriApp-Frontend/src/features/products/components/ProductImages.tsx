// --------------------------
// ProductImages Component

import { useState } from "react";

// --------------------------
interface ProductImagesProps {
  images: string[];
  name: string;
}

function ProductImages({ images, name }: ProductImagesProps) {
  const [mainImage, setMainImage] = useState(images[0]);

  return (
    <div className="flex flex-col items-center w-full">
      {/* Main Image */}
      <div className="w-full flex justify-center items-center mb-4">
        <img
          src={mainImage}
          alt={name}
          className="w-full h-auto max-h-[400px] object-contain rounded-lg shadow-md"
          loading="lazy"
        />
      </div>

      {/* Thumbnails */}
      {images.length > 1 && (
        <div className="flex gap-2 mt-2 overflow-x-auto w-full">
          {images.map((img, index) => (
            <img
              key={index}
              src={img}
              alt={`Thumbnail ${index + 1}`}
              className={`w-20 h-20 object-cover rounded cursor-pointer flex-shrink-0 transition-all hover:ring-2 hover:ring-green-500 ${
                mainImage === img ? "ring-2 ring-green-500" : ""
              }`}
              loading="lazy"
              onClick={() => setMainImage(img)}
            />
          ))}
        </div>
      )}
    </div>
  );
}

export default ProductImages;
