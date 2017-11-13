import React from 'react'
import PropTypes from 'prop-types'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import MainPane from '../components/MainPane'
import * as Actions from '../actions'

const App = ({ view, textEntryTokens, selectedEntryId, actions }) => (
    <MainPane 
        view={view}
        selectedEntryId={selectedEntryId}
        textEntryTokens={textEntryTokens}
        changeView={actions.changeView}
        selectEntry={actions.selectEntry}
        fetchTextEntry={actions.fetchTextEntry}     
	/>
);

App.propTypes = {
    view: PropTypes.string.isRequired,
    textEntryTokens: PropTypes.array.isRequired,   
    selectedEntryId: PropTypes.string.isRequired,
    actions: PropTypes.object.isRequired
}

const mapStateToProps = state => ({
    view: state.view,
    textEntryTokens: state.textEntry.textEntryTokens,
    selectedEntryId: state.selectedEntryId
});

const mapDispatchToProps = dispatch => ({
    actions: bindActionCreators(Actions, dispatch)
});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(App)
