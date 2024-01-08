import React from "react";
import classNames from "classnames";

export type TextInputProps = {
  type?: "text" | "password" | "date";
  variant?: "default" | "warning" | "error";
  className?: string;
  label: string;
};

// Input related classes
const containerClasses = "relative my-6 group";
const inputClasses = "outline-none px-3 py-3 peer w-full";

// Label related classes
const labelTextClasses =
  "text-sm text-gray-500 pointer-events-none group-focus-within:!text-sm peer-placeholder-shown:text-base";
const labelPositionClasses =
  "absolute left-[9px] top-px transform -translate-y-1/2 peer-placeholder-shown:top-1/2 group-focus-within:!top-px";
const labelAnimationClasses = "transition-all duration-50 px-1";
const labelClasses = classNames(
  labelTextClasses,
  labelPositionClasses,
  labelAnimationClasses
);

// Fieldset and Legend related classes
const baseFieldsetClasses =
  "inset-0 absolute border border-gray-400 rounded pointer-events-none mt-[-9px]";
const hoverFieldsetClasses = "group-hover:border-gray-700";
const focusFieldsetClasses = "group-focus-within:border-2";
const baseLegendClasses = "ml-2 text-sm invisible whitespace-nowrap";
const transitionFieldsetClasses = classNames(
  baseFieldsetClasses,
  hoverFieldsetClasses,
  focusFieldsetClasses,
  "invisible peer-placeholder-shown:visible"
);
const transitionLegendClasses = classNames(
  baseLegendClasses,
  "px-0 transition-all duration-50 max-w-[0.01px] group-focus-within:max-w-full group-focus-within:px-1"
);
const notchFieldsetClasses = classNames(
  baseFieldsetClasses,
  hoverFieldsetClasses,
  focusFieldsetClasses,
  "visible peer-placeholder-shown:invisible"
);
const notchLegendClasses = classNames(baseLegendClasses, "px-1 max-w-full");

// Variants
const variantColorClasses = {
  default: {
    label: "group-focus-within:!text-primary-500",
    focusFieldset: "group-focus-within:!border-primary-500",
  },
  warning: {
    label: "group-focus-within:!text-orange-500",
    focusFieldset: "group-focus-within:!border-orange-500",
  },
  error: {
    label: "group-focus-within:!text-red-500",
    focusFieldset: "group-focus-within:!border-red-500",
  },
} as const;

export const TextInput: React.FC<TextInputProps> = ({
  label,
  className,
  type = "text",
  variant = "default",
}: TextInputProps) => {
  return (
    <div className={classNames(containerClasses, className)}>
      <input type={type} className={inputClasses} placeholder=" " />

      <label
        className={classNames(labelClasses, variantColorClasses[variant].label)}
      >
        {label}
      </label>

      <fieldset
        className={classNames(
          transitionFieldsetClasses,
          variantColorClasses[variant].focusFieldset
        )}
      >
        <legend className={transitionLegendClasses}>{label}</legend>
      </fieldset>

      <fieldset
        className={classNames(
          notchFieldsetClasses,
          variantColorClasses[variant].focusFieldset
        )}
      >
        <legend className={notchLegendClasses}>{label}</legend>
      </fieldset>
    </div>
  );
};
