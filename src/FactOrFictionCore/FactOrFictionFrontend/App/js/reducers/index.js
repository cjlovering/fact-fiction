import { combineReducers } from 'redux'
import view from './view'
import textEntry from './textEntry'
import selectedEntryId from './selectedEntryId'

const root = combineReducers({
    view,
    textEntry,
    selectedEntryId
})

export default root;
