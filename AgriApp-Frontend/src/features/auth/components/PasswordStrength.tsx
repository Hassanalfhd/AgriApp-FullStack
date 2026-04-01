interface Props {
  password: string;
}

const getStrength = (password: string) => {
  let score = 0;
  if (password.length >= 8) score++;
  if (/[A-Z]/.test(password)) score++;
  if (/[0-9]/.test(password)) score++;
  if (/[^A-Za-z0-9]/.test(password)) score++;

  return score;
};

const PasswordStrength = ({ password }: Props) => {
  const strength = getStrength(password);

  const labels = ["Weak", "Fair", "Good", "Strong"];
  const colors = ["bg-red-500", "bg-yellow-500", "bg-blue-500", "bg-green-600"];

  return (
    <div className="mt-1">
      <div className="h-2 w-full bg-gray-200 rounded">
        <div
          className={`h-2 rounded transition-all ${colors[strength - 1] || ""}`}
          style={{ width: `${(strength / 4) * 100}%` }}
        />
      </div>
      {password && (
        <p className="text-sm mt-1 text-gray-600">
          Password strength: <b>{labels[strength - 1] || "Very weak"}</b>
        </p>
      )}
    </div>
  );
};

export default PasswordStrength;
