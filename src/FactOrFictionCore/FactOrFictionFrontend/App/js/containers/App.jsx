import React from 'react'
import PropTypes from 'prop-types'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import MainPane from '../components/MainPane'
import * as Actions from '../actions'

const App = ({ view, textEntryTokens, actions }) => (
    <div>
        <MainPane 
			view={view}
            textEntryTokens={textEntryTokens} 
            changeView={actions.changeView}
            fetchTextEntry={actions.fetchTextEntry}
		/>
    </div>
);

App.propTypes = {
    view: PropTypes.string.isRequired,
    textEntryTokens: PropTypes.array.isRequired,        
    actions: PropTypes.object.isRequired
}

const mapStateToProps = state => ({
    view: state.view,
    textEntryTokens: state.textEntry.textEntryTokens
});

const mapDispatchToProps = dispatch => ({
    actions: bindActionCreators(Actions, dispatch)
});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(App)
