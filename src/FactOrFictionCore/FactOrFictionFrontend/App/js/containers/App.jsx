import React from 'react'
import PropTypes from 'prop-types'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import MainPane from '../components/MainPane'
import * as Actions from '../actions'

const App = ({
    tokens, 
    isFetching, 
    isDoneFetchingFeed, 
    didInvalidate, 
    textEntryTokenIds, 
    feedTokenIds, 
    view, 
    selectedEntryId, 
    details, 
    detailsShown,
    votes,
    similarTokenIds,
    isFetchingSimilar,
    actions
}) => (
    <MainPane 
        tokens={tokens}
        isFetching={isFetching}
        isDoneFetchingFeed={isDoneFetchingFeed}
        didInvalidate={didInvalidate}
        textEntryTokenIds={textEntryTokenIds}
        feedTokenIds={feedTokenIds}
        view={view}
        selectedEntryId={selectedEntryId}
        details={details}
        votes={votes}
        detailsShown={detailsShown}
        similarTokenIds={similarTokenIds}
        isFetchingSimilar={isFetchingSimilar}
        changeView={actions.changeView}
        selectEntry={actions.selectEntry}
        fetchTextEntry={actions.fetchTextEntry}
        fetchFeedTokens={actions.fetchFeedTokens}
        fetchDetails={actions.fetchDetails}
        showDetails={actions.showDetails}
        castVote={actions.castVote}
        fetchSimilarTokens={actions.fetchSimilarTokens}
	/>
);

App.propTypes = {
    tokens: PropTypes.object.isRequired,
    isFetching: PropTypes.bool.isRequired,
    didInvalidate: PropTypes.bool.isRequired,
    textEntryTokenIds: PropTypes.array.isRequired,
    feedTokenIds: PropTypes.array.isRequired,
    view: PropTypes.string.isRequired,
    selectedEntryId: PropTypes.string.isRequired,
    details: PropTypes.object.isRequired,
    detailsShown: PropTypes.object.isRequired,
    votes: PropTypes.object.isRequired,
    similarTokenIds: PropTypes.object.isRequired, 
    isFetchingSimilar: PropTypes.bool.isRequired,
    actions: PropTypes.object.isRequired
}

const mapStateToProps = state => ({
    ...state.textEntry,
    details: state.sentenceDetails.details,
    detailsShown: state.detailsShown,
    view: state.view,
    selectedEntryId: state.selectedEntryId,
    similarTokenIds: state.similarTokens.similarTokenIds,
    isFetchingSimilar: state.similarTokens.isFetchingSimilar
});

const mapDispatchToProps = dispatch => ({
    actions: bindActionCreators(Actions, dispatch)
});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(App)
