import { combineReducers } from 'redux';
import view from './view';
import sentences from './sentences';
import selectedEntryId from './selectedEntryId';
import sentenceDetails from './sentenceDetails';
import detailsShown from './detailsShown';
import similarSentences from './similarSentences';

const root = combineReducers({
    view,
    sentences,
    selectedEntryId,
    sentenceDetails,
    detailsShown,
    similarSentences
})

export default root;
