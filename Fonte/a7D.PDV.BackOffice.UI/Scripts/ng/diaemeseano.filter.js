angular.module('backoffice')
  .filter('diaemeseano', diaemeseano);

diaemeseano.$inject = ["$window"]

function diaemeseano($window) {
  return function(x) {
    return $window.moment(x).format("DD/MM/YYYY");
  }
}