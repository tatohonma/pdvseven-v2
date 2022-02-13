angular.module('backoffice')
  .filter('diasemana', diasemana);

diasemana.$inject = ["$window"]

function diasemana($window) {
  return function(x) {
    return $window.moment(x).format("dddd");
  }
}