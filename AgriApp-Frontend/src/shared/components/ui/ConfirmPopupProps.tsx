import React from "react";

interface ConfirmPopupProps {
  isOpen: boolean; // هل Popup ظاهر
  title?: string; // عنوان Popup
  message: string; // نص الرسالة
  onConfirm: () => void; // عند الضغط على موافق
  onCancel: () => void; // عند الضغط على إلغاء
}

const ConfirmPopup: React.FC<ConfirmPopupProps> = ({
  isOpen,
  title = "تأكيد العملية",
  message,
  onConfirm,
  onCancel,
}) => {
  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
      <div className="bg-white p-6 rounded-xl shadow-lg w-[300px]">
        {title && (
          <h2 className="text-lg font-semibold mb-4 text-center">{title}</h2>
        )}
        <p className="text-center mb-6">{message}</p>

        <div className="flex justify-between">
          <button className="px-4 py-2 bg-gray-300 rounded" onClick={onCancel}>
            إلغاء
          </button>
          <button
            className="px-4 py-2 bg-red-600 text-white rounded"
            onClick={onConfirm}
          >
            موافق
          </button>
        </div>
      </div>
    </div>
  );
};

export default ConfirmPopup;
