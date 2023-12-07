var getTokenButton = document.getElementById('getToken');
var tokenOutput = document.getElementById('tokenOutput');

getTokenButton.addEventListener('click', function (event) {
	event.preventDefault();

	var login = document.getElementById('login-input').value;
	var password = document.getElementById('password-input').value;

	var url = 'http://visual_mvc:80/api/auth?login=' + login + '&password=' + password;
	console.log(url);
	fetch(url)
		.then(response => {
			if (!response.ok) {
				throw new Error(response.body);
			}
			return response.text();
		})
		.then(data => {
			tokenOutput.value = data;
		})
		.catch(error => {
			tokenOutput.value = error;
		});
});