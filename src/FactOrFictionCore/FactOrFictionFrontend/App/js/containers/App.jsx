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
        changeView={actions.changeView}
        selectEntry={actions.selectEntry}
        fetchTextEntry={actions.fetchTextEntry}
        fetchFeedTokens={actions.fetchFeedTokens}
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
    actions: PropTypes.object.isRequired
}

const mapStateToProps = state => ({
    ...state.textEntry,
    view: state.view,
    selectedEntryId: state.selectedEntryId
});

const mapDispatchToProps = dispatch => ({
    actions: bindActionCreators(Actions, dispatch)
});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(App)
