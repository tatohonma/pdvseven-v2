angular.module('backoffice')
  .filter('relativo', relativo);

relativo.$inject = ["$window"]

function relativo($window) {
  return function(x) {
    return $window.moment(x).fromNow();
  }
}