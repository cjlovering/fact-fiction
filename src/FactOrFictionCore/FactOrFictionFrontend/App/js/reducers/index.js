import { combineReducers } from 'redux';
import view from './view';
import textEntry from './textEntry';
import selectedEntryId from './selectedEntryId';
import sentenceDetails from './sentenceDetails';
import detailsShown from './detailsShown';

const root = combineReducers({
    view,
    textEntry,
    selectedEntryId,
    sentenceDetails,
    detailsShown
})

export default root;
