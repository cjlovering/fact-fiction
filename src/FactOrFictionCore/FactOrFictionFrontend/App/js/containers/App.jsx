import React from 'react'
import PropTypes from 'prop-types'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import MainPane from '../components/MainPane'
import * as Actions from '../actions'

const App = ({
    sentences, 
    isFetching, 
    isDoneFetchingFeed, 
    didInvalidate, 
    textEntrySentenceIds, 
    feedSentenceIds, 
    view, 
    selectedEntryId, 
    details, 
    detailsShown,
    votes,
    isFetchingSimilar,
    similarSentenceIds,
    textEntry,
    actions
}) => (
    <MainPane 
        sentences={sentences}
        isFetching={isFetching}
        isDoneFetchingFeed={isDoneFetchingFeed}
        didInvalidate={didInvalidate}
        textEntrySentenceIds={textEntrySentenceIds}
        feedSentenceIds={feedSentenceIds}
        view={view}
        selectedEntryId={selectedEntryId}
        details={details}
        votes={votes}
        detailsShown={detailsShown}
        isFetchingSimilar={isFetchingSimilar}
        similarSentenceIds={similarSentenceIds}
        textEntry={textEntry}
        changeView={actions.changeView}
        selectEntry={actions.selectEntry}
        fetchTextEntry={actions.fetchTextEntry}
        fetchFeedSentences={actions.fetchFeedSentences}
        fetchDetails={actions.fetchDetails}
        showDetails={actions.showDetails}
        castVote={actions.castVote}
        fetchSimilarSentences={actions.fetchSimilarSentences}
	/>
);

App.propTypes = {
    sentences: PropTypes.object.isRequired,
    isFetching: PropTypes.bool.isRequired,
    didInvalidate: PropTypes.bool.isRequired,
    textEntrySentenceIds: PropTypes.array.isRequired,
    feedSentenceIds: PropTypes.array.isRequired,
    view: PropTypes.string.isRequired,
    selectedEntryId: PropTypes.string.isRequired,
    details: PropTypes.object.isRequired,
    detailsShown: PropTypes.object.isRequired,
    votes: PropTypes.object.isRequired,
    isFetchingSimilar: PropTypes.bool.isRequired,
    similarSentenceIds: PropTypes.object.isRequired, 
    textEntry: PropTypes.string.isRequired,
    actions: PropTypes.object.isRequired
}

const mapStateToProps = state => ({
    ...state.sentences,
    details: state.sentenceDetails.details,
    detailsShown: state.detailsShown,
    view: state.view,
    selectedEntryId: state.selectedEntryId,
    similarSentenceIds: state.similarSentences.similarSentenceIds,    
    isFetchingSimilar: state.similarSentences.isFetchingSimilar,
    textEntry: state.textEntry,
});

const mapDispatchToProps = dispatch => ({
    actions: bindActionCreators(Actions, dispatch)
});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(App)
