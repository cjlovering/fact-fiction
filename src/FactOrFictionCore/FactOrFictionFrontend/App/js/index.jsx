import 'babel-polyfill';

import React from 'react';
import thunkMiddleware from 'redux-thunk'

import { render } from 'react-dom';
import { createStore, applyMiddleware } from 'redux';
import { Provider } from 'react-redux'
import { createLogger } from 'redux-logger'

import reducer from './reducers';
import App from './containers/App';

const loggerMiddleware = createLogger()

const store = createStore(reducer,
	applyMiddleware(
		thunkMiddleware // lets us dispatch() functions
	  )
);
const rootElement = document.getElementById('content');

render(  
	<Provider store={store}>
		<App />
	</Provider>,
    rootElement
);


