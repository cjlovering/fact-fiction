import React from 'react'
import PropTypes from 'prop-types'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import MainPane from '../components/MainPane'
import * as Actions from '../actions'

const App = ({ view, facts, actions }) => (
    <div>
        <MainPane 
			view={view}
            facts={facts} 
            addFact={actions.addFact}
            changeView={actions.changeView}
		/>
    </div>
);

App.propTypes = {
    view: PropTypes.string.isRequired,
    facts: PropTypes.array.isRequired,    
    actions: PropTypes.object.isRequired
}

const mapStateToProps = state => ({
    view: state.view,
    facts: state.facts
});

const mapDispatchToProps = dispatch => ({
    actions: bindActionCreators(Actions, dispatch)
});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(App)
