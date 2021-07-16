(function () {
    angular.
        module('clientApp').
        component('artistList', {
            
            controller: function artistListController(artistService, $http) {
                var self = this;
                self.artists = [];

                self.loadArtists = function () {
                    artistService.getList().then(
                        // on success, set state to received data
                        function (res) {
                            console.log(res.data);
                            self.artists = res.data;
                        },
                        // on fail, just console.log
                        function () {
                            console.log('Error receiving data')
                        }
                    );
                }

                self.loadArtists();

            },
            templateUrl: "/components/artist-list/artist-list.template.html" 
        });
})();


