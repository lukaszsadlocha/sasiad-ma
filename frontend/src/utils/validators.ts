export const validators = {
  email: (email: string): boolean => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  },

  password: (password: string): boolean => {
    return password.length >= 8;
  },

  required: (value: string): boolean => {
    return value.trim().length > 0;
  },

  minLength: (value: string, min: number): boolean => {
    return value.trim().length >= min;
  },

  maxLength: (value: string, max: number): boolean => {
    return value.trim().length <= max;
  },

  phone: (phone: string): boolean => {
    const phoneRegex = /^\+?[\d\s-()]+$/;
    return phoneRegex.test(phone) && phone.replace(/\D/g, '').length >= 10;
  },

  url: (url: string): boolean => {
    try {
      new URL(url);
      return true;
    } catch {
      return false;
    }
  }
};

export const validateForm = <T extends Record<string, any>>(
  data: T,
  rules: Record<keyof T, Array<(value: any) => boolean | string>>
): Record<keyof T, string> => {
  const errors = {} as Record<keyof T, string>;

  Object.keys(rules).forEach((field) => {
    const value = data[field];
    const fieldRules = rules[field];

    for (const rule of fieldRules) {
      const result = rule(value);
      if (typeof result === 'string') {
        errors[field] = result;
        break;
      }
      if (result === false) {
        errors[field] = `Invalid ${String(field)}`;
        break;
      }
    }
  });

  return errors;
};