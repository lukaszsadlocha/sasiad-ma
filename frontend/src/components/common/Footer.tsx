import React from 'react';

const Footer: React.FC = () => {
  const currentYear = new Date().getFullYear();

  return (
    <footer className="bg-gray-800 text-white mt-auto">
      <div className="container mx-auto px-4 py-6">
        <div className="flex flex-col md:flex-row justify-between items-center">
          <div className="mb-4 md:mb-0">
            <h3 className="text-lg font-semibold">Sasiad-Ma</h3>
            <p className="text-sm text-gray-300">
              Strengthening neighborhood bonds through sharing
            </p>
          </div>

          <div className="flex flex-col md:flex-row gap-6 text-sm">
            <div className="flex gap-4">
              <a
                href="#"
                className="text-gray-300 hover:text-white transition-colors"
              >
                About
              </a>
              <a
                href="#"
                className="text-gray-300 hover:text-white transition-colors"
              >
                Privacy
              </a>
              <a
                href="#"
                className="text-gray-300 hover:text-white transition-colors"
              >
                Terms
              </a>
              <a
                href="#"
                className="text-gray-300 hover:text-white transition-colors"
              >
                Contact
              </a>
            </div>
          </div>
        </div>

        <div className="mt-6 pt-6 border-t border-gray-700 text-center text-sm text-gray-400">
          <p>&copy; {currentYear} Sasiad-Ma. All rights reserved.</p>
        </div>
      </div>
    </footer>
  );
};

export default Footer;