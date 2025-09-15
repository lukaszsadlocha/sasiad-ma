import React from 'react';

const ItemsPage: React.FC = () => {
  return (
    <div className="container mx-auto px-4 py-8">
      <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-8 text-center">
        <h1 className="text-2xl font-bold text-gray-900 mb-4">Items</h1>
        <p className="text-gray-600 mb-6">
          This page will show available items to borrow and your own items to lend.
        </p>
        <div className="text-sm text-gray-500">
          🚧 Under development - Item management features coming soon!
        </div>
      </div>
    </div>
  );
};

export default ItemsPage;
