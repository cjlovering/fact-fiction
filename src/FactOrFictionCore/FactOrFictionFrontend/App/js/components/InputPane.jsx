import React from 'react';
import PropTypes from 'prop-types';
import _ from '../../stylesheets/components/_InputPane.scss';

export default class InputPane extends React.Component {
	constructor(props) {
	    super(props);
		this.state = {
			textEntry: "Enter some text (news article...)!"
		}
	}

    render() {
        return (
            <div>
				<div>
                    <textarea 
                        className="input-box"
						rows="10" 
						value={this.state.textEntry}
						onChange={e => this.setState({ textEntry: e.target.value })}
					/>
				</div>
                <button
                    className="start-button ms-Button"
                    id="get-data-from-selection"
                    onClick={() => this.props.addFact(this.state.textEntry)}
                >
                    <span className="ms-Button-label">Start</span>
                </button>
			</div>
        );
    }
}

InputPane.propTypes = {
    addFact: PropTypes.func.isRequired
}