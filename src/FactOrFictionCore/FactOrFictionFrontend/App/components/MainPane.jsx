import React from 'react'
import PropTypes from 'prop-types';
import InputPane from './InputPane';
import ListView from './ListView';
import _ from '../stylesheets/components/_MainPane.scss';

export default class MainPane extends React.Component {
    render() {
        return (
            <div>
                <h2> Placeholder!! </h2>
                <InputPane addFact={this.props.addFact} />
                <ListView facts={this.props.facts} />
            </div>
        )
    }
}

MainPane.propTypes = {
    addFact: PropTypes.func.isRequired,
	facts: PropTypes.array.isRequired
}