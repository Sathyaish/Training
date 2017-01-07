$(document).ready(function() {
	
	var app = {
		params : 
		{
			client_id : '760921016908-32jtmqtojn7aja9qvt4t7mh0089er9tb.apps.googleusercontent.com', 
			redirect_uri : 'https://localhost:443/OAuth2/index.html',
			response_type : 'token',
			scope : 'https://www.googleapis.com/auth/plus.login https://www.googleapis.com/auth/userinfo.email',
			state : 'abcd'
		},
		
		provider : 
		{
			name: 'Google',
			authServerUrl: 'https://accounts.google.com/o/oauth2/auth?',
			resourceServerUrl: 'https://www.googleapis.com/plus/v1/people/me?'
		},
		
		fields : 'emails,displayName,name(givenName)',
		httpMethod: 'GET'
	};

	var page = new Page(app);
	
	page.display();
});