(function () {
    'use strict';

    angular.module('artist.service', [])
        .factory('artistService', [
            '$http',
            function ($http) {
                var apiUrl = '/artist';

                return {
                    // get all artists
                    getList: function () {
                        // returns promise
                        return $http.get(`${apiUrl}/list`);
                    },

                    // get artist by ID
                    getById: function (id) {
                        return $http.get(`${apiUrl}/${id}`);
                    },

                    // create artist
                    create: function (artistData) {
                        console.log('calling artistService.create', artistData)
                        return $http({
                            url: `${apiUrl}/create`,
                            method: 'POST',
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            data: JSON.stringify(artistData)
                        });
                    },

                    // update artist by ID
                    update: function (artistId, artistData) {
                        console.log('calling artistService.update', artistId, artistData)
                        return $http({
                            url: `${apiUrl}/edit/${artistId}`,
                            method: 'POST',
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            data: JSON.stringify(artistData)
                        });
                    },

                    // delete artist
                    delete: function (id) {
                        return $http.get(`${apiUrl}/delete/${id}`);
                    },
                }
            }
        ])
    
})();