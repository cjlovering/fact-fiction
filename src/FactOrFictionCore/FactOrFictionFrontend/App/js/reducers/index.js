import { combineReducers } from 'redux';
import view from './view';
import textEntry from './textEntry';
import selectedEntryId from './selectedEntryId';
import sentenceDetails from './sentenceDetails';
import detailsShown from './detailsShown';
import similarTokens from './similarTokens';

const root = combineReducers({
    view,
    textEntry,
    selectedEntryId,
    sentenceDetails,
    detailsShown,
    similarTokens
})

export default root;
