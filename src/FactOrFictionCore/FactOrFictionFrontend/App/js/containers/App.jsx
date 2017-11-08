import React from 'react'
import PropTypes from 'prop-types'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import MainPane from '../components/MainPane'
import * as Actions from '../actions'

const App = ({ facts, actions }) => (
    <div>
        <MainPane 
			facts={facts} 
			addFact={actions.addFact}
		/>
    </div>
);

App.propTypes = {
    facts: PropTypes.array.isRequired,
    actions: PropTypes.object.isRequired
}

const mapStateToProps = state => ({
    facts: state
});

const mapDispatchToProps = dispatch => ({
    actions: bindActionCreators(Actions, dispatch)
});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(App)
