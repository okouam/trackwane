(function() {

	class NotificationsLocations {

		constructor() {
		}

	}

	NotificationsLocations.$inject = [];


	angular
		.module("trackwane")
		.component('notificationsLocations', {
			controller: NotificationsLocations,
			template: (
				<div>
					SHOW LOCATIONS FOR ORGANIZATION
				</div>
			)
		});
})();
