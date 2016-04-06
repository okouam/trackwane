import angular from 'angular'
import Controller from './controller'

const MODULE_NAME = 'trackwane.application.pages.dashboard';

angular
	.module(MODULE_NAME, [])
	.controller('trackwane.application.pages.dashboard.controller', Controller);

export default MODULE_NAME
