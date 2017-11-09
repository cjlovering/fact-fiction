import React from 'react';

import { render } from 'react-dom';
import { createStore } from 'redux';
import { Provider } from 'react-redux'

import reducer from './reducers';
import App from './containers/App';

const store = createStore(reducer);
const rootElement = document.getElementById('content');

render(  
	<Provider store={store}>
		<App />
	</Provider>,
    rootElement
);


