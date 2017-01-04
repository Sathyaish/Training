$(document).ready(function() {
	
	var app = {
		params : 
		{
			client_id : '1654158091578578', 
			redirect_uri : 'https://localhost:443/OAuth2/index.html',
			response_type : 'token',
			scope : 'public_profile email',
			state : 'abcd'
		},
		
		provider : 
		{
			name: 'Facebook',
			authServerUrl: 'https://www.facebook.com/dialog/oauth?',
			resourceServerUrl: 'https://graph.facebook.com/me?'
		},
		
		fields : 'name,first_name,last_name,email',
		httpMethod: 'GET'
	};

	var page = new Page(app);
	
	page.display();
});