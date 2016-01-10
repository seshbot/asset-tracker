(function () {
    'use strict';

    angular.module('app', ['angular-loading-bar']);
    angular.module('app').controller('NewDeliveryController', NewDeliveryController);
    angular.module('app').controller('NewCollectionController', NewCollectionController);
    angular.module('app').controller('NewCleaningController', NewCleaningController);

    NewDeliveryController.$inject = ['$scope', '$http', 'assetsAvailable', 'addSuccessCallback'];
    function NewDeliveryController($scope, $http, assetsAvailable, addSuccessCallback) {
        $scope.assetsAvailable = assetsAvailable;

        // TODO: cannot create a delivery with more than one fulfillment through the warehouse UI currently
        $scope.newDelivery = {
            manualOrderDetails: false,
            orderId: -1,
            orderSourceId: '',
            orderCustomerSourceId: '',
            assets: []
        };

        $scope.newAsset = {
            serialNumber: '',
            assetType: {
                description: ''
            }
            //build: function () {
            //    return {
            //        serialNumber: this.serialNumber,
            //        assetType: {
            //            description: this.description
            //        }
            //    };
            //}
        };

        $scope.selectedAssetId = -1;
        $scope.selectedAsset = null;
        $scope.removeAsset = removeAsset;
        $scope.addSelectedAsset = addSelectedAsset;
        $scope.canAddNewAsset = canAddNewAsset;
        $scope.addNewAsset = addNewAsset;
        $scope.canSend = canSend;
        $scope.send = send;

        $scope.$watch('selectedAssetId', handleSelectedAssetIdChanged);

        function removeAsset(asset) {
            if (null == asset) return;
            moveAsset(asset, $scope.newDelivery.assets, $scope.assetsAvailable);
        };

        function addSelectedAsset() {
            var asset = $scope.selectedAsset;

            if (null == asset) return;
            moveAsset(asset, $scope.assetsAvailable, $scope.newDelivery.assets);
        };

        function canAddNewAsset() {
            var asset = $scope.newAsset;
            return asset.serialNumber !== '';
        };

        function addNewAsset() {
            if (!canAddNewAsset()) return;
            // clone - we could also use jQuery.extend({}, $scope.newAsset);
            var asset = JSON.parse(JSON.stringify($scope.newAsset));
            moveAsset(asset, [], $scope.newDelivery.assets);
        };

        function handleSelectedAssetIdChanged(newValue, oldValue) {
            var newAsset = null;
            for (var idx = 0; idx < $scope.assetsAvailable.length; ++idx) {
                var asset = $scope.assetsAvailable[idx];
                if (asset.id == $scope.selectedAssetId) {
                    newAsset = asset;
                    break;
                }
            }

            $scope.selectedAsset = newAsset;
        };

        function canSend() {
            var orderSpecified = $scope.newDelivery.manualOrderDetails
                ? ($scope.newDelivery.orderSourceId !== '')
                : ($scope.newDelivery.orderId != -1);

            var assetsSpecified = $scope.newDelivery.assets.length > 0;

            return orderSpecified && assetsSpecified;
        };

        function send() {
            if (!canSend()) return;

            var delivery = {
                orderFulfillments: [ $scope.newDelivery, ],
            };
            $http.post('/api/deliveries/', delivery)
                 .then(handleAddSuccess, handleAddFailure);

            function handleAddSuccess(response) {
                console.log('added successfully');
                addSuccessCallback();
            };

            function handleAddFailure(response) {
                console.log('error getting API data', response);
                if (!angular.isObject(response.data) || !response.data.message) {
                    $q.reject({
                        statusMessage: response.statusText,
                        errorMessage: 'An unknown error occurred',
                        errorMessageDetail: ''
                    });
                } else {
                    $q.reject({
                        statusText: response.statusText,
                        message: response.data.message,
                        messageDetail: response.data.messageDetail
                    });
                }
            };
        };
    }

    NewCollectionController.$inject = ['$scope', '$http', 'assetsWithCustomers', 'addSuccessCallback'];
    function NewCollectionController($scope, $http, assetsWithCustomers, addSuccessCallback) {
        $scope.assetsWithCustomers = assetsWithCustomers;

        $scope.newCollection = {
            assets: []
        };

        $scope.selectedAssetId = -1;
        $scope.selectedAsset = null;
        $scope.removeAsset = removeAsset;
        $scope.addSelectedAsset = addSelectedAsset;
        $scope.canSend = canSend;
        $scope.send = send;

        $scope.$watch('selectedAssetId', handleSelectedAssetIdChanged);

        function removeAsset(asset) {
            if (null == asset) return;
            moveAsset(asset, $scope.newCollection.assets, $scope.assetsWithCustomers);
        };

        function addSelectedAsset() {
            var asset = $scope.selectedAsset;

            if (null == asset) return;
            moveAsset(asset, $scope.assetsWithCustomers, $scope.newCollection.assets);
        };

        function handleSelectedAssetIdChanged(newValue, oldValue) {
            var newAsset = null;
            for (var idx = 0; idx < $scope.assetsWithCustomers.length; ++idx) {
                var asset = $scope.assetsWithCustomers[idx];
                if (asset.id == $scope.selectedAssetId) {
                    newAsset = asset;
                    break;
                }
            }

            $scope.selectedAsset = newAsset;
        };

        function canSend() {            
            var result =
               ($scope.newCollection.assets.length > 0);

            return result;
        };

        function send() {
            if (!canSend()) return;
            $http.post('/api/collections/', $scope.newCollection)
                 .then(handleAddSuccess, handleAddFailure);

            function handleAddSuccess(response) {
                console.log('added successfully');
                addSuccessCallback();
            };

            function handleAddFailure(response) {
                console.log('error getting API data', response);
                if (!angular.isObject(response.data) || !response.data.message) {
                    $q.reject({
                        statusMessage: response.statusText,
                        errorMessage: 'An unknown error occurred',
                        errorMessageDetail: ''
                    });
                } else {
                    $q.reject({
                        statusText: response.statusText,
                        message: response.data.message,
                        messageDetail: response.data.messageDetail
                    });
                }
            };
        };
    }

    NewCleaningController.$inject = ['$scope', '$http', 'assetsNeedCleaning', 'addSuccessCallback'];
    function NewCleaningController($scope, $http, assetsNeedCleaning, addSuccessCallback) {
        $scope.assetsNeedCleaning = assetsNeedCleaning;

        $scope.newCleaning = {
            assets: []
        };

        $scope.selectedAssetId = -1;
        $scope.selectedAsset = null;
        $scope.removeAsset = removeAsset;
        $scope.addSelectedAsset = addSelectedAsset;
        $scope.canSend = canSend;
        $scope.send = send;

        $scope.$watch('selectedAssetId', handleSelectedAssetIdChanged);

        function removeAsset(asset) {
            if (null == asset) return;
            moveAsset(asset, $scope.newCleaning.assets, $scope.assetsNeedCleaning);
        };

        function addSelectedAsset() {
            var asset = $scope.selectedAsset;

            if (null == asset) return;
            moveAsset(asset, $scope.assetsNeedCleaning, $scope.newCleaning.assets);
        };

        function handleSelectedAssetIdChanged(newValue, oldValue) {
            var newAsset = null;
            for (var idx = 0; idx < $scope.assetsNeedCleaning.length; ++idx) {
                var asset = $scope.assetsNeedCleaning[idx];
                if (asset.id == $scope.selectedAssetId) {
                    newAsset = asset;
                    break;
                }
            }

            $scope.selectedAsset = newAsset;
        };

        function canSend() {
            return $scope.newCleaning.assets.length > 0;
        };

        function send() {
            if (!canSend()) return;

            $http.post('/api/cleaning/', $scope.newCleaning)
                 .then(handleAddSuccess, handleAddFailure);

            function handleAddSuccess(response) {
                console.log('added successfully');
                addSuccessCallback();
            };

            function handleAddFailure(response) {
                console.log('error getting API data', response);
                if (!angular.isObject(response.data) || !response.data.message) {
                    $q.reject({
                        statusMessage: response.statusText,
                        errorMessage: 'An unknown error occurred',
                        errorMessageDetail: ''
                    });
                } else {
                    $q.reject({
                        statusText: response.statusText,
                        message: response.data.message,
                        messageDetail: response.data.messageDetail
                    });
                }
            };
        };
    }

    function moveAsset(asset, fromArray, toArray) {
        for (var idx = 0; idx < fromArray.length; ++idx) {
            if (asset == fromArray[idx]) {
                fromArray.splice(idx, 1);
                break;
            }
        }

        toArray.push(asset);
        toArray.sort(function (a, b) {
            if (!a.hasOwnProperty('id')) return -1;
            if (!b.hasOwnProperty('id')) return 1;
            return a.id - b.id
        });
    };
})();