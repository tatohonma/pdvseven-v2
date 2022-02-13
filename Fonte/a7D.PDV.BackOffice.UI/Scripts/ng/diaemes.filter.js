angular.module('backoffice')
  .filter('diaemes', diaemes);

diaemes.$inject = ["$window"]

function diaemes($window) {
  return function(x) {
    return $window.moment(x).format("DD/MM");
  }
}