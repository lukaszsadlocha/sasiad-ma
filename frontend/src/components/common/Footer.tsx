import React from 'react';
import { useTranslation } from 'react-i18next';

const Footer: React.FC = () => {
  const currentYear = new Date().getFullYear();
  const { t } = useTranslation();

  return (
    <footer className="bg-gray-800 text-white mt-auto">
      <div className="container mx-auto px-4 py-6">
        <div className="flex flex-col md:flex-row justify-between items-center">
          <div className="mb-4 md:mb-0">
            <h3 className="text-lg font-semibold">Sasiad-Ma</h3>
            <p className="text-sm text-gray-300">
              {t('footer.tagline')}
            </p>
          </div>

          <div className="flex flex-col md:flex-row gap-6 text-sm">
            <div className="flex gap-4">
              <a
                href="#"
                className="text-gray-300 hover:text-white transition-colors"
              >
                {t('footer.about')}
              </a>
              <a
                href="#"
                className="text-gray-300 hover:text-white transition-colors"
              >
                {t('footer.privacy')}
              </a>
              <a
                href="#"
                className="text-gray-300 hover:text-white transition-colors"
              >
                {t('footer.terms')}
              </a>
              <a
                href="#"
                className="text-gray-300 hover:text-white transition-colors"
              >
                {t('footer.contact')}
              </a>
            </div>
          </div>
        </div>

        <div className="mt-6 pt-6 border-t border-gray-700 text-center text-sm text-gray-400">
          <p>{t('footer.copyright', { year: currentYear })}</p>
        </div>
      </div>
    </footer>
  );
};

export default Footer;