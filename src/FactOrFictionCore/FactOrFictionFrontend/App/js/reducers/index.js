import { combineReducers } from 'redux'
import view from './view'
import textEntry from './textEntry'

const root = combineReducers({
    view,
    textEntry
})

export default root;
